# Notification Service

## Descrizione

Il **Notification Service** è un microservizio responsabile della gestione delle notifiche degli utenti.

Consente agli utenti autenticati di visualizzare le proprie notifiche, consultare quelle non lette e contrassegnarle come lette.

Le notifiche vengono generate automaticamente in risposta agli eventi prodotti dagli altri microservizi del sistema, in particolare dall'**Offer Service** e dal **Visit Service**.

## Architettura

Il servizio fa parte di un'architettura a microservizi:

* Espone API REST
* Tutte le operazioni sono protette tramite autenticazione JWT
* Comunica con il **User Service** per verificare l'esistenza degli utenti
* Consuma eventi Apache Kafka provenienti dall'**Offer Service** e dal **Visit Service**
* Memorizza lo storico delle notifiche degli utenti

## Avvio del servizio

Per avviare il servizio in locale:

```bash
# esempio
dotnet run
```

Oppure con Docker:

```bash
docker-compose up
```

Il servizio sarà disponibile su:

```text
http://localhost:7805
```

## API

Documentazione Swagger disponibile qui:

```text
http://localhost:7805/swagger/index.html
```

## Autenticazione e autorizzazione

Il servizio utilizza **JWT (JSON Web Token)** per proteggere tutte le operazioni.

### Accesso autenticato

Tutte le operazioni richiedono un token JWT valido.

Il client deve includere il token nell'header HTTP:

```http
Authorization: Bearer <token>
```

### Regole di autorizzazione

#### Visualizzazione delle notifiche

Un utente autenticato può:

* visualizzare tutte le proprie notifiche
* visualizzare le proprie notifiche non lette
* consultare una specifica notifica

Non è consentito accedere alle notifiche di altri utenti.

#### Lettura di una notifica

Una notifica può essere contrassegnata come letta solo se:

* appartiene all'utente autenticato

## Struttura delle notifiche

Ogni notifica contiene informazioni relative a un evento avvenuto nel sistema.

Tra le informazioni principali:

* utente destinatario
* messaggio della notifica
* data di creazione
* stato di lettura

## Stati di una notifica

| Stato  | Descrizione                           |
| ------ | ------------------------------------- |
| Unread | Notifica non ancora letta             |
| Read   | Notifica già visualizzata dall'utente |

Le nuove notifiche vengono create automaticamente come **Unread**.

## Endpoints principali

| Metodo | Endpoint                     | Autenticazione | Descrizione                                         |
| ------ | ---------------------------- | -------------- | --------------------------------------------------- |
| GET    | /api/notifications           | ✅ Sì           | Recupera tutte le notifiche dell'utente autenticato |
| GET    | /api/notifications/unread    | ✅ Sì           | Recupera le notifiche non lette                     |
| GET    | /api/notifications/{id}      | ✅ Sì           | Recupera una notifica specifica                     |
| PATCH  | /api/notifications/{id}/read | ✅ Sì           | Contrassegna una notifica come letta                |

## Integrazioni

### User Service

Il Notification Service utilizza il User Service per:

* verificare l'esistenza degli utenti destinatari
* recuperare le informazioni necessarie per l'invio delle notifiche

### Kafka

#### Eventi Kafka consumati

| Topic        | Evento         | Provenienza   | Descrizione                                                  |
| ------------ | -------------- | ------------- | ------------------------------------------------------------ |
| offer-events | OfferCreated   | Offer Service | Notifica la creazione di una nuova offerta                   |
| offer-events | OfferAccepted  | Offer Service | Notifica l'accettazione di un'offerta                        |
| offer-events | OfferRejected  | Offer Service | Notifica il rifiuto di un'offerta                            |
| visit-events | VisitCreated   | Visit Service | Notifica la creazione di una nuova richiesta di visita       |
| visit-events | VisitConfirmed | Visit Service | Notifica la conferma di una visita da parte del proprietario |
| visit-events | VisitCancelled | Visit Service | Notifica il rifiuto di una visita da parte del proprietario  |

#### Gestione degli eventi

Il Notification Service ascolta gli eventi pubblicati dall'Offer Service e dal Visit Service e genera automaticamente notifiche per gli utenti coinvolti.

Ad esempio:

* quando viene creata un'offerta, il proprietario dell'immobile può ricevere una notifica
* quando un'offerta viene accettata, l'autore dell'offerta riceve una notifica di conferma
* quando un'offerta viene rifiutata, l'autore dell'offerta riceve una notifica di rifiuto
* quando viene richiesta una visita, il proprietario dell'immobile può ricevere una notifica
* quando una visita viene confermata, il visitatore riceve una notifica di conferma
* quando una visita viene rifiutata, il visitatore riceve una notifica di rifiuto

## Controlli automatici

* Le notifiche vengono create con stato iniziale **Unread**.
* La data di creazione viene valorizzata automaticamente dal sistema.
* Un utente può accedere esclusivamente alle proprie notifiche.
* Una notifica letta viene automaticamente contrassegnata come **Read**.
* Tutti i controlli di autorizzazione si basano sull'utente autenticato contenuto nel JWT.

## Flusso degli eventi

1. L'Offer Service o il Visit Service pubblica un evento Kafka.
2. Il Notification Service riceve un evento supportato.
3. Viene identificato l'utente destinatario.
4. Viene creata una nuova notifica.
5. La notifica diventa disponibile tramite le API REST.
6. L'utente può consultarla e contrassegnarla come letta.

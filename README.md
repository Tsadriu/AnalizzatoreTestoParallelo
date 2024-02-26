# AnalizzatoreTestoParallelo

## Descrizione

L'obiettivo di questo progetto è sviluppare un'applicazione in C# che analizza testi provenienti da file di grandi
dimensioni per estrarre informazioni statistiche e di analisi. L'applicazione utilizza la programmazione parallela per
migliorare l'efficienza del processo di analisi, dividendo il carico di lavoro tra più thread/processi.

## Caratteristiche Principali

### Lettura e Analisi del Testo

Il programma legge testi da file e calcola statistiche come il conteggio delle parole, la frequenza delle parole e la
lunghezza media delle frasi.

### Parallelizzazione

Vengono impiegate tecniche di parallelizzazione, come l'uso di Parallel LINQ (PLINQ) in C#, per distribuire il lavoro
tra più thread o processi, migliorando le prestazioni dell'analisi.

### Interfaccia Utente

L'applicazione offre un'interfaccia utente semplice che consente all'utente di selezionare file di testo e visualizzare
i risultati dell'analisi.

### Misurazione delle Prestazioni

Il programma include una funzionalità per misurare e confrontare le prestazioni dell'analisi sia in modalità seriale che
parallela, evidenziando i benefici dell'approccio parallelo.
# StoreExportReport
A GUI application to manage store daily report

Il progetto è realizzato utilizzando i seguenti linguaggi di programmazione:
-C#
-Go
-Python

A supporto sono state utilizzate le seguenti tecnologie:
-IDE di sviluppo Visual Studio e Visual Studio code
-Framework .Net v.4.7.2
-Libreria Newtonsoft.Json v.13.0.3 (installato tramite Visual Studio)
 Per installare dall'ide andare su Progetto -> Gestisci pacchetti NuGet e cercare Newtonsoft.Json
-Libreria pandas
 Assicurarsi di avere installato Python e pip(versione >= 19.3) e lanciare il comando pip install pandas al path dell'interprete Python

Per avviare il progetto in locale bisogna aprirlo su Visual Studio.
Verificare la classe Constants e, se necessario, cambiare i path degli script e degli interpreti Go e Python ( al momento sono configurati su C:\Users\Carlo\source\repos\StoreExportReport\)
Creare la cartella per l'Upload del file(al moento è C:\Upload\)
Creare la cartella per la History dei vecchi report(al moento è C:\Upload\History\)
Avviare il progetto in debug o generare un eseguibile.

Per configurare un nuovo negozio bisogna seguire le specifiche indicate nel file SER_Specifiche_file_negozi-1.0, reperibile all'interno della cartella documentazione.

Dei file di test sono inseriti all'interno della cartella TestFile

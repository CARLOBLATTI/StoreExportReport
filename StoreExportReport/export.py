import json
import pandas as pd
from datetime import datetime
import os
import shutil

#installare pandas con pip install pandas

#funzione per gestire lo spostamento dei file  
def move_file(source_directory, destination_directory, shop_name):
    # Lista dei file nella cartella sorgente
    files = os.listdir(source_directory)
    
    for file in files:
        if file.startswith(shop_name):
            # Costruiamo i percorsi completi per il file sorgente e di destinazione
            source_path = os.path.join(source_directory, file)
            destination_path = os.path.join(destination_directory, file)
            
            # Se un file con lo stesso nome esiste già nella cartella di destinazione, lo eliminiamo
            if os.path.exists(destination_path):
                os.remove(destination_path)
            
            # Spostiamo il file nella cartella di destinazione
            shutil.move(source_path, destination_path)
            
            print(f"Il file {file} è stato spostato con successo!")

baseFilePath = "C:/Upload/"
historyDirecorty = "History/"
tempJsonFile = "test.json"
with open(baseFilePath + tempJsonFile) as json_file:
    data = json.load(json_file)
    

#print(data)
shopName = data['Nome_Negozio']

tickets = data['Scontrini']

# Creazione di un DataFrame da JSON
dfDetail = pd.DataFrame(tickets)

current_date = datetime.now().strftime('%Y%m%d')

#remove any special characters or spaces to ensure a valid file name.
sanitized_shop_name = ''.join(e for e in shopName if e.isalnum())

file = baseFilePath + f"{sanitized_shop_name}_{current_date}.xlsx"
fileDetail = baseFilePath + f"{sanitized_shop_name}_{current_date}_dettaglio.xlsx"

# Rimuovere i campi desiderati dal dizionario
del data['Scontrini']
df = pd.DataFrame([data])

#Sposto gli ultimi report del negozio nella history
destination_directory = baseFilePath + historyDirecorty + shopName
move_file(baseFilePath, destination_directory,shopName)

# Salvataggio del DataFrame in un file Excel
df.to_excel(file, index=False)
dfDetail.to_excel(fileDetail, index=False)

  



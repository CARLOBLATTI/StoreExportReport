package main

import (
	"encoding/csv"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"math"
	"os"
	"strconv"
	"strings"
	"time"
)

type SingleTicket struct {
	Categoria    string
	Descrizione  string
	Prezzo_Lordo float64
	Data         time.Time
	Codice_Iva   int
	Prezzo_Netto float64
	Iva          float64
	Id_Cassa     int
	Id_Scontrino int
	Id_Societa   int
	Id_Negozio   int
	Quantita     int
}

type MainReport struct {
	Nome_Negozio        string
	Numero_Prodotti     int
	Prezzo_Lordo_Totale float64
	Prezzo_Netto_Totale float64
	Iva_Totale          float64
	Scontrini           []SingleTicket
}

func readCsvFile(filePath string) [][]string {
	f, err := os.Open(filePath)
	if err != nil {
		log.Fatal("Unable to read input file "+filePath, err)
	}
	defer f.Close()

	csvReader := csv.NewReader(f)
	csvReader.Comma = ';'
	csvReader.FieldsPerRecord = -1
	records, err := csvReader.ReadAll()
	if err != nil {
		log.Fatal("Unable to parse file as CSV for "+filePath, err)
	}

	return records
}

func generateReport(records [][]string) MainReport {

	leg := len(records)

	//shopName := records[0][0]
	numerOfLines := records[leg-1][0]
	fmt.Println(numerOfLines)

	mainReport := MainReport{}
	mainReport.Nome_Negozio = strings.TrimSpace(records[0][0])

	var productsNumber int
	var totalPrice float64
	var totalPriceWithoutVat float64
	var totalVat float64
	var tickets []SingleTicket

	for _, value := range records {
		var ticket SingleTicket
		if len(value) < 10 {
			continue // skip short records
		}

		productDescription := strings.Split(value[6], "-")

		//Categoria
		category := productDescription[0]
		//Descrizione
		description := productDescription[1]
		//ID società
		societyId, _ := strconv.Atoi(value[0])
		//ID negozio
		shopId, _ := strconv.Atoi(value[1])
		//ID cassa
		cashDeskId, _ := strconv.Atoi(value[2])
		//ID scontrino
		ticketId, _ := strconv.Atoi(value[3])
		//Quantità
		quantity, _ := strconv.Atoi(value[8])
		//Codice iva
		vatCode, _ := strconv.Atoi(value[9])

		productsNumber += quantity

		price, _ := strconv.ParseFloat(value[7], 64)
		totalPrice += price

		vat := price * float64(vatCode) / 100
		totalVat += vat

		priceWithoutVat := price - vat
		totalPriceWithoutVat += priceWithoutVat
		timeString := value[4] + " " + value[5]

		ticketDate, err := time.Parse("2006/01/02 15:04", timeString)
		if err != nil {
			fmt.Println("Could not parse time:", err)
		}

		ticket.Categoria = strings.TrimSpace(category)
		ticket.Descrizione = strings.TrimSpace(description)
		ticket.Data = ticketDate
		ticket.Id_Societa = societyId
		ticket.Id_Negozio = shopId
		ticket.Id_Cassa = cashDeskId
		ticket.Id_Scontrino = ticketId
		ticket.Prezzo_Lordo = truncate(price, 2)
		ticket.Quantita = quantity
		ticket.Codice_Iva = vatCode
		ticket.Iva = truncate(vat, 2)
		ticket.Prezzo_Netto = truncate(priceWithoutVat, 2)

		tickets = append(tickets, ticket)

	}

	mainReport.Numero_Prodotti = productsNumber
	mainReport.Prezzo_Lordo_Totale = truncate(totalPrice, 2)
	mainReport.Prezzo_Netto_Totale = truncate(totalPriceWithoutVat, 2)
	mainReport.Iva_Totale = truncate(totalVat, 2)
	mainReport.Scontrini = tickets

	return mainReport

}

// the function will return a truncated version of the number with the specified number of decimals.
func truncate(number float64, decimals int) float64 {
	factor := math.Pow(10, float64(decimals))
	return math.Trunc(number*factor) / factor
}

func main() {
	const tempCsvPath = "C:/Upload/temp.csv"
	const tempJsonPath = "C:/Upload/test.json"
	records := readCsvFile(tempCsvPath)
	mainReport := generateReport(records)

	//b, err := json.Marshal(mainReport)
	b, err := json.MarshalIndent(mainReport, "", "")
	if err != nil {
		fmt.Println(err)
		return
	}
	fmt.Println(string(b))
	_ = ioutil.WriteFile(tempJsonPath, b, 0644)
}

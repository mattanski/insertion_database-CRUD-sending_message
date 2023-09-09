using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using PrimoEsempio.Agenda;


namespace PrimoEsempio
{
    internal class Program
        {
            public static void Main(string[] args)
            {
                AgendaInterface i = new AgendaInterface();
                Console.WriteLine("Benvenuto nella tua agenda personale");
                
                do
                {
                    
                    Console.WriteLine("Premi N se vuoi inserire un nuovo contatto oppure R " +
                                      "se vuoi cercare un contatto esistente");
                
                    string result = Console.ReadLine();
                    
                    if (result == "R")
                    {
                        Console.WriteLine("Inserisci il nome da ricercare");
                        string nome = Console.ReadLine();
                        Console.WriteLine("Inserisci il cognome da ricercare");
                        string cognome = Console.ReadLine();
                        i.CercaEMostraContatti(nome, cognome);
                        
                        i.SelezionaContatto();

                        Console.WriteLine("Premi U se vuoi aggiornare il contatto selezionato, " +
                                          "D se vuoi cancellarlo oppure, I se vuoi mandare un  messaggio o" +
                                          " un tasto qualsiasi per tornare al menu principale");

                        string operazione = Console.ReadLine();

                        if (operazione == "U")
                        {
                            i.AggiornaContatto();
                            
                        }else if (operazione == "D")
                        {
                            i.EliminaContatto();
                        }else if (operazione == "I")
                        {
                            i.InviaMessaggio();
                        }
                    }
                    else
                    {
                        ContattoDto nuovContattoDto = i.RichiediDatiContatto();
                        i.CreaNuovoContatto(nuovContattoDto);
                        Console.WriteLine("Contatto inserito correttamente");
                    }
                } while (i.VuoiContinuareAdUtilizzareLAgenda());
            }


        }
}
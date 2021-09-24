SELECT        LEDGERACCOUNT AS Kontoanzeigename, TRANSACTIONCURRENCYCODE AS Währung, TRANSACTIONCURRENCYCREDITAMOUNT AS Soll, TRANSACTIONCURRENCYDEBITAMOUNT AS Haben, 
                         REPORTINGCURRENCYAMOUNT AS Buchungsbetrag, CONVERT(DATE, ACCOUNTINGDATE) 
                         AS Buchungsdatum, DOCUMENTNUMBER AS Beleg, NULL AS Zeile, /* CONVERT(INT, LINENUMBER) AS Zeile ,*/ DESCRIPTION AS Belegtext, CAST('<TOKEN>' + REPLACE(LEDGERACCOUNT, '-', '</TOKEN><TOKEN>') 
                         + '</TOKEN>' AS XML) AS [Hauptkonto Schlüssel XML], RECID
FROM            dbo.GeneralJournalAccountEntryStaging)
    
    
    SELECT Kontoanzeigename, Währung, Soll, Haben, Buchungsbetrag, Buchungsdatum, Beleg, Zeile, Belegtext, Hauptkonto, Standort, Ertragsstelle, Projekt
     FROM            (SELECT        RECID, Kontoanzeigename, Währung, MAX(Soll) AS Soll, MAX(Haben) AS Haben, MAX(Buchungsbetrag * BFBALANCEFACTOR) AS Buchungsbetrag, Buchungsdatum, Beleg, Zeile, Belegtext, Hauptkonto, Standort, 
                                                         Ertragsstelle, Projekt
                               FROM            (SELECT        Kontoanzeigename, Währung, Soll, Haben, Buchungsbetrag, Buchungsdatum, Beleg, Zeile, Belegtext, [Hauptkonto Schlüssel XML].value('/TOKEN[1]', 'varchar(7)') AS Hauptkonto, 
                                                                                   [Hauptkonto Schlüssel XML].value('/TOKEN[2]', 'varchar(2)') AS Standort, [Hauptkonto Schlüssel XML].value('/TOKEN[3]', 'varchar(4)') AS Ertragsstelle, [Hauptkonto Schlüssel XML].value('/TOKEN[4]', 
                                                                                   'varchar(50)') AS Projekt, RECID
                                                         FROM            CTR) AS CTR_1 JOIN
                                                             (SELECT        [ACCOUNTID] AS BFACCOUNTID, [FACTOR] AS BFBALANCEFACTOR
                                                               FROM            [dbo].[PbiAccountFactor]) AS BF ON CTR_1.Hauptkonto = BF.BFACCOUNTID
                               GROUP BY CTR_1.RECID, Kontoanzeigename, Währung, Buchungsdatum, Beleg, Zeile, Belegtext, Hauptkonto, Standort, Ertragsstelle, Projekt) AS GROUPEDTABLE

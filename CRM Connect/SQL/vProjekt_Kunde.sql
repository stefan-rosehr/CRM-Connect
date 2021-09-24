﻿USE [Refirm]
GO

/****** Object:  View [dbo].[vProjekt_Kunde]    Script Date: 06.04.2018 14:24:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vProjekt_Kunde]
AS
SELECT        Projekte_1.cNr, Projekte_1.cBeschreibung, dbo.Benutzer.cVorname, dbo.Benutzer.cName, dbo.Benutzer.cInitialien, dbo.Projekte.cNr AS cNrSub, dbo.Projekte.cBeschreibung AS cBeschreibungSub, dbo.Adressen.cKNr, 
                         dbo.Adressen.cFirma, dbo.Adressen.cStr, dbo.Adressen.cPLZ, dbo.Adressen.cOrt, dbo.Adressen.cTel, dbo.Adressen.cEmail, dbo.Projekte.Status_ID
FROM            dbo.Projekte INNER JOIN
                         dbo.Projekte AS Projekte_1 ON dbo.Projekte.Parent_ID = Projekte_1.ID LEFT OUTER JOIN
                         dbo.Benutzer ON Projekte_1.ProjektLeiter_ID = dbo.Benutzer.id LEFT OUTER JOIN
                         dbo.Adressen ON dbo.Projekte.RechnungsAdresse_ID = dbo.Adressen.ID
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Projekte"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 215
               Right = 253
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "Projekte_1"
            Begin Extent = 
               Top = 231
               Left = 107
               Bottom = 469
               Right = 322
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Benutzer"
            Begin Extent = 
               Top = 0
               Left = 651
               Bottom = 397
               Right = 897
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Adressen"
            Begin Extent = 
               Top = 3
               Left = 328
               Bottom = 420
               Right = 536
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 2760
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      En' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vProjekt_Kunde'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'd
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vProjekt_Kunde'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vProjekt_Kunde'
GO



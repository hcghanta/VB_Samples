Imports System.IO
Imports System.Text
Imports FileHelpers
Imports Microsoft.Office.Interop.Excel

Public Class Form1


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.Title = "Please select a DB file"
        OpenFileDialog1.InitialDirectory = "D:\Visual Basic\"
        OpenFileDialog1.Filter = "CSV Files|*.csv"
        Dim fpath As String = ""
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            fpath = OpenFileDialog1.FileName
        End If

        Dim EngOrders As FileHelperEngine(Of Orders) = New FileHelperEngine(Of Orders)
        Dim res = EngOrders.ReadFile(fpath)
        'Bind res to DataGridView1
        PathTextBox.Text = fpath
        'using data table
        For Each item In res
            DataGridView1.Rows.Add(item.Id, item.Item, item.Owner, item.Quantity, item.Unknown, item.SellingPrice, item.MSRP, item.Manufacturer, item.Category, item.Discount)
        Next

    End Sub

    Private Sub ExportButton_Click(sender As Object, e As EventArgs) Handles ExportButton.Click
        Dim xlApp As Microsoft.Office.Interop.Excel.Application
        Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook
        Dim xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim i As Integer
        Dim j As Integer

        xlApp = New Application
        xlWorkBook = xlApp.Workbooks.Add(misValue)
        xlWorkSheet = xlWorkBook.Sheets("sheet1")


        For i = 0 To DataGridView1.RowCount - 1
            For j = 0 To DataGridView1.ColumnCount - 1
                For k As Integer = 1 To DataGridView1.Columns.Count
                    xlWorkSheet.Cells(1, k) = DataGridView1.Columns(k - 1).HeaderText
                    xlWorkSheet.Cells(i + 2, j + 1) = DataGridView1(j, i).Value.ToString()
                Next
            Next
        Next

        xlWorkSheet.SaveAs("D:\Visual Basic\Exported Data\vbexcel.xlsx")
        xlWorkBook.Close()
        xlApp.Quit()

        releaseObject(xlApp)
        releaseObject(xlWorkBook)
        releaseObject(xlWorkSheet)

        MsgBox("You can find the file D:\vbexcel.xlsx")
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

End Class



<DelimitedRecord(",")>
Public Class Orders
    Public Property Id As Integer
    Public Property Item As String
    Public Property Owner As String
    Public Property Quantity As Integer
    Public Property Unknown As Double
    Public Property SellingPrice As Double
    Public Property MSRP As Double
    Public Property Manufacturer As String
    Public Property Category As String
    Public Property Discount As Double
End Class

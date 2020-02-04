Imports System.IO
Imports System.Text
Imports FileHelpers

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        openFileDialog1.Title = "Please select a DB file"
        OpenFileDialog1.InitialDirectory = "D:\Visual Basic\"
        OpenFileDialog1.Filter = "CSV Files|*.csv"
        Dim fpath As String = ""
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            fpath = OpenFileDialog1.FileName
        End If

        Dim EngOrders As FileHelperEngine(Of Orders) = New FileHelperEngine(Of Orders)
        Dim res = EngOrders.ReadFile(fpath)

        PathTextBox.Text = fpath
        Dim fname As String = fpath
        Dim threader As New StreamReader(fname, Encoding.Default)
        Dim sline As String = ""

        Do
            sline = threader.ReadLine
            If sline Is Nothing Then Exit Do
            If sline.Contains("""") Then
                Dim fquote As Integer = sline.IndexOf("""")
                Dim lquote As Integer = sline.LastIndexOf("""")
                Dim slineLength As Integer = sline.Length

                Dim sub1 As String = sline.Substring(0, fquote)
                Dim sub2 As String = sline.Substring(fquote, lquote - 2).Replace(",", "")
                Dim sub3 As String = sline.Substring(lquote, slineLength - lquote)

                sline = sub1 + sub2 + sub3
            End If

            Dim words() As String = sline.Split(",")
            Dim value As String = ""
            DataGridView1.Rows.Add("")

            For ix As Integer = 0 To 9
                Dim currentrow As String = DataGridView1.Rows.Count - 1
                DataGridView1.Rows(currentrow).Cells(ix).Value = words(ix)
            Next

            If Not IsNumeric(words(0)) Then DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(0).Style.BackColor = Color.Yellow
        Loop
        DataGridView1.Rows.Add("")
        threader.Close()

    End Sub

    Private Sub ExportButton_Click(sender As Object, e As EventArgs) Handles ExportButton.Click
        Dim StrExport As String = ""
        For Each C As DataGridViewColumn In DataGridView1.Columns
            StrExport &= """" & C.HeaderText & ""","
        Next
        StrExport = StrExport.Substring(0, StrExport.Length - 1)
        StrExport &= Environment.NewLine

        For Each R As DataGridViewRow In DataGridView1.Rows
            For Each C As DataGridViewCell In R.Cells
                If Not C.Value Is Nothing Then
                    StrExport &= """" & C.Value.ToString & ""","
                Else
                    StrExport &= """" & "" & ""","
                End If
            Next
            StrExport = StrExport.Substring(0, StrExport.Length - 1)
            StrExport &= Environment.NewLine
        Next

        Dim tw As IO.TextWriter = New IO.StreamWriter("D:\Visual Basic\exportfile.csv")
        tw.Write(StrExport)
        tw.Close()
        MessageBox.Show("File Saved in ""D: \Visual Basic\exportfile.csv""")
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

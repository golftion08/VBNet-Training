Imports System.Drawing
Imports System.IO
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json

Public Class GrandSubTotal
    Inherits System.Web.UI.Page

    Private GrandTotal As Decimal = 0
    Private SubTotal As Decimal = 0
    Private Model_SubTotal As Decimal = 0

    Private CurrentBrand As String = ""
    Private CurrentModel As String = ""

    Private CurrentIndex As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GridView1.Visible = False

        Dim DataTable As DataTable = ReadDataFromJsonFile()
        If DataTable.Rows.Count <> 0 Then
            GridView1.DataSource = DataTable
            GridView1.DataBind()
            GridView1.Visible = True
        End If

    End Sub

    Private Function ReadDataFromJsonFile() As DataTable
        Dim JsonPath As String = "P:\VBNet Project\VBNet Training\App\GrandSubTotal\Data.json"
        ' Read the JSON file
        Dim jsonContent As String
        Using Reader As New StreamReader(JsonPath)
            jsonContent = Reader.ReadToEnd()
        End Using

        ' Deserialize the JSON content using Json.NET
        'Dim carSales = JsonConvert.DeserializeObject(Of List(Of CarSales))(jsonContent)

        'Convert a JSON data to a DataTable 
        Dim DataTable As DataTable = JsonConvert.DeserializeObject(Of DataTable)(jsonContent)
        DataTable.DefaultView.Sort = "Brand ASC, Model ASC, Quarter ASC"
        Return DataTable
    End Function

    Private Sub AddSubTotalRow(ByVal Text As String, ByVal Value As String, ByVal Type As Integer)
        Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal)

        Select Case Type
            Case 1
                row.BackColor = ColorTranslator.FromHtml("#F9F9F9")
                row.Cells.AddRange(New TableCell(3) {New TableCell(),
                New TableCell() With {
                  .Text = "",
                  .HorizontalAlign = HorizontalAlign.Left
                },
                 New TableCell() With {
                  .Text = Text,
                  .HorizontalAlign = HorizontalAlign.Left
                }, New TableCell() With {
                  .Text = Value,
                  .HorizontalAlign = HorizontalAlign.Right
                }})
            Case 2
                row.BackColor = ColorTranslator.FromHtml("#F9F9F9")
                row.Cells.AddRange(New TableCell(3) {New TableCell(),
                New TableCell() With {
                  .Text = Text,
                  .HorizontalAlign = HorizontalAlign.Left
                },
                 New TableCell() With {
                  .Text = "",
                  .HorizontalAlign = HorizontalAlign.Right
                }, New TableCell() With {
                  .Text = Value,
                  .HorizontalAlign = HorizontalAlign.Right
                }})
            Case Else
        End Select
        GridView1.Controls(0).Controls.Add(row)
    End Sub

    Private Sub GridView1_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim Sales As String = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Sales"))
            Dim PreviousBrand As String = DataBinder.Eval(e.Row.DataItem, "Brand").ToString()
            Dim PreviousModel As String = DataBinder.Eval(e.Row.DataItem, "Model").ToString()

            If e.Row.RowIndex > 0 Then

                If Not CurrentModel.Equals(PreviousModel) Or Not CurrentBrand.Equals(PreviousBrand) Then
                    AddSubTotalRow("Subtotal of " & CurrentBrand & " : " & CurrentModel & "", Model_SubTotal.ToString("N2"), 2)
                    Model_SubTotal = 0
                    CurrentModel = PreviousModel
                    CurrentIndex = e.Row.RowIndex 'To get the last model row for handling, show the last row subtotal.
                End If

                If Not CurrentBrand.Equals(PreviousBrand) Then
                    AddSubTotalRow("Subtotal of " & CurrentBrand & "", SubTotal.ToString("N2"), 1)
                    GrandTotal += SubTotal
                    SubTotal = 0
                    Model_SubTotal = 0
                    CurrentBrand = PreviousBrand
                    CurrentModel = PreviousModel
                    CurrentIndex = e.Row.RowIndex ' To get the last model row for handling, show the last row subtotal.
                End If
            Else ' Assign the first row data (Brand , Model) to the variables
                CurrentBrand = PreviousBrand
                CurrentModel = PreviousModel
            End If

            SubTotal = SubTotal + Sales
            Model_SubTotal += Sales
        End If

    End Sub

    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        Model_SubTotal = 0
        For I As Integer = CurrentIndex To GridView1.Rows.Count - 1
            Model_SubTotal += Convert.ToDecimal(GridView1.Rows(I).Cells(3).Text)
        Next
        Me.AddSubTotalRow("Subtotal of " & CurrentBrand & " : " & CurrentModel & "", Model_SubTotal.ToString("N2"), 2)
        Me.AddSubTotalRow("Subtotal of " & CurrentBrand & "", SubTotal.ToString("N2"), 1)
        GrandTotal += SubTotal
        Me.AddSubTotalRow("Grandtotal of sales", GrandTotal.ToString("N2"), 1)
    End Sub

    Public Class CarSales
        Public Property Brand As String
        Public Property Model As String
        Public Property Quarter As Integer
        Public Property Sales As Integer
    End Class

End Class
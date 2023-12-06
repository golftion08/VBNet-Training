Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json

Public Class GrandSubTotal
    Inherits System.Web.UI.Page

    Private MainDataTable As New DataTable

    Private GrandTotal As Decimal = 0
    Private SubToTal As Decimal = 0
    Private Model_SubTotal As Decimal = 0

    Private CurrentBrand As String = ""
    Private CurrentModel As String = ""

    Private CurrentIndex As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            BindGrid()
        End If

    End Sub

    Private Function ReadDataFromJsonFile() As DataTable

        Dim JsonPath As String = "D:\Web Servers\Training\App\GrandSubTotal\Data.json"
        Dim JsonContent As String
        Using Reader As New StreamReader(JsonPath)
            JsonContent = Reader.ReadToEnd
        End Using

        Dim DataTable As DataTable = JsonConvert.DeserializeObject(Of DataTable)(JsonContent)
        DataTable.DefaultView.Sort = "Brand ASC, Model ASC , Quarter ASC"
        Return DataTable
    End Function

    Private Sub BindGrid()
        GridView1.Visible = False
        MainDataTable = ReadDataFromJsonFile()

        If MainDataTable.Rows.Count > 0 Then
            GridView1.DataSource = MainDataTable
            GridView1.DataBind()
            GridView1.Visible = True
        End If
    End Sub

    Private Sub AddRow(ByVal Text As String, ByVal Value As String, ByVal Type As Integer)
        Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal)

        Select Case Type
            Case 1
                row.BackColor = ColorTranslator.FromHtml("#FFDFDF")
                row.Cells.AddRange(New TableCell(3) {
                New TableCell() With {
                    .Text = Text,
                    .HorizontalAlign = HorizontalAlign.Left
                },
                New TableCell() With {
                    .Text = "",
                    .HorizontalAlign = HorizontalAlign.Left
                },
                New TableCell() With {
                    .Text = "",
                    .HorizontalAlign = HorizontalAlign.Left
                },
                New TableCell() With {
                    .Text = Value,
                    .HorizontalAlign = HorizontalAlign.Right
                }})

            Case 2
                row.BackColor = ColorTranslator.FromHtml("#FFF6F6")
                row.Cells.AddRange(New TableCell(3) {New TableCell(),
                New TableCell() With {
                    .Text = Text,
                    .HorizontalAlign = HorizontalAlign.Left
                },
                New TableCell() With {
                    .Text = "",
                    .HorizontalAlign = HorizontalAlign.Left
                },
                New TableCell() With {
                    .Text = Value,
                    .HorizontalAlign = HorizontalAlign.Right
                }})

            Case Else
        End Select
        GridView1.Controls(0).Controls.Add(row)
    End Sub
    Private Sub GridView1_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Sales As Decimal = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Sales"))
            Dim PreviousBrand As String = DataBinder.Eval(e.Row.DataItem, "Brand").ToString
            Dim PreviousModel As String = DataBinder.Eval(e.Row.DataItem, "Model").ToString()

            If e.Row.RowIndex > 0 Then

                If Not CurrentModel.Equals(PreviousModel) Or Not CurrentBrand.Equals(PreviousBrand) Then
                    AddRow("Subtotal of " & CurrentBrand & " : " & CurrentModel & "", Model_SubTotal.ToString("N2"), 2)
                    Model_SubTotal = 0
                    CurrentModel = PreviousModel
                    CurrentIndex = e.Row.RowIndex 'To get the last model row for handling, show the last row subtotal.
                End If

                If Not CurrentBrand.Equals(PreviousBrand) Then
                    AddRow("Subtotal of " & CurrentBrand & "", SubToTal.ToString("N2"), 1)
                    GrandTotal += SubToTal
                    SubToTal = 0
                    Model_SubTotal = 0
                    CurrentBrand = PreviousBrand
                    CurrentModel = PreviousModel
                    CurrentIndex = e.Row.RowIndex ' To get the last model row for handling, show the last row subtotal.
                End If
            Else ' Assign the first row data (Brand , Model) to the variables
                CurrentBrand = PreviousBrand
                CurrentModel = PreviousModel
            End If

            SubToTal = SubToTal + Sales
            Model_SubTotal += Sales
        End If
    End Sub

    'Handling the last row not showing
    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        Model_SubTotal = 0
        For I As Integer = CurrentIndex To GridView1.Rows.Count - 1
            Model_SubTotal += Convert.ToDecimal(GridView1.Rows(I).Cells(3).Text)
        Next
        AddRow("Subtotal of " & CurrentBrand & " : " & CurrentModel & "", Model_SubTotal.ToString("N2"), 2)
        AddRow("Subtotal of " & CurrentBrand & "", SubToTal.ToString("N2"), 1)
        GrandTotal += SubToTal
        AddRow("Grandtotal of sales", GrandTotal.ToString("N2"), 1)
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center

            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).Text = Decimal.Parse(e.Row.Cells(3).Text).ToString("N2")
        End If
    End Sub
End Class
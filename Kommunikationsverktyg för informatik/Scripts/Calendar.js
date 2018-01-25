$(document).ready(function ()
{
    $("#nextMonth").on("click", function ()
    {
        var month = parseInt($("#currentMonth").val()) + 1;
        $.ajax({
            url: '/Calendar/nextMonth',
            data: {newMonth: month},
            type: "GET",
            success: function (data) {
                $('#Month').html(data);
            },
            error: function (xhr, status, error) {
                var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
                $('#Month').html(msg);
            },
            complete: function (xhr, status) {
                var doneMsg = "Operation complete with status: " + status;
                alert(doneMsg);
            }
        });
        //var url = $(this).data('request-url');
        //alert(url);
        //$("#Month").load(url);
        //$("#Month").load('@Html.Raw(Url.Action(" nextMonth", "CalendarController")) ');
        //var url = $(this).data('request-url');
        //$("#Month").load('<%= Url.Action("_CalendarPartial", "Calendar")%>', function (responseTxt, statusTxt, xhr) {
        //    if (statusTxt == "success")
        //        alert("External content loaded successfully!");
        //    if (statusTxt == "error")
        //        alert("Error: " + xhr.status + ": " + xhr.statusText);
        //});
    });
});
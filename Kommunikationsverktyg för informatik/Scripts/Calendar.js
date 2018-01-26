$(document).ready(function ()
{
    $("#nextMonth").on("click", function ()
    {
        var month = parseInt($("#currentMonth").val()) + 1;
        if(month > 12)
        {
            var year = parseInt($("#currentYear").val()) + 1;
        }
        else
        {
            var year = parseInt($("#currentYear").val());
        }
        $.ajax({
            url: '/Calendar/nextMonth',
            data:
            {
                newMonth: month,
                newYear: year
            },
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
    });
});
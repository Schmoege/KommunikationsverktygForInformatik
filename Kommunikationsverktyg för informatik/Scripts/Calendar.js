$(document).ready(function ()
{
    /*==============================================================================================
        Shows next month
    ==============================================================================================*/
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
            }
        });
    });

    /*==============================================================================================
        Shows previous month
    ==============================================================================================*/
    $("#previousMonth").on("click", function ()
    {
        var month = parseInt($("#currentMonth").val() - 1);
        if(month <= 0)
        {
            var year = parseInt($("#currentYear").val() - 1);
        }
        else
        {
            var year = parseInt($("#currentYear").val());
        }
        $.ajax({
            url: '/Calendar/previousMonth',
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
            }
        });
    });

    /*==============================================================================================
        Shows the selected day
    ==============================================================================================*/
    $(document).on("click", ".element__item", function ()
    {
        var year = $(this).data("year");
        var month = $(this).data("month");
        var day = $(this).data("day");
        $.ajax({
            url: '/Calendar/day',
            data:
            {
                year: year,
                month: month,
                day: day
            },
            type: "GET",
            success: function (data) {
                $(".pointer").hide();
                $('#Month').html(data);
            },
            error: function (xhr, status, error) {
                var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
                alert(msg);
            }
        });
    });
    /*==============================================================================================
        Back to the callendar from day
    ==============================================================================================*/
    $(document).on("click", "#backToCal", function ()
    {
        var year = parseInt($("#year").val());
        var month = parseInt($("#month").val());
        $.ajax({
            url: '/Calendar/Month',
            data:
            {
                year: year,
                month: month
            },
            type: "GET",
            success: function (data)
            {
                $(".pointer").show();
                $('#Month').html(data);
            },
            error: function (xhr, status, error)
            {
                var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
                $('#Month').html(msg);
            }
        });
    });
});
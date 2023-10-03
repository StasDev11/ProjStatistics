$(document).ready(function () {
    var count = 0;
    var html = '';

    $.ajax({
        type: 'POST',
        url: '/Home/GetSourcesStatistics',
        contentType: "application/json; charset=utf-8",
        data: "{}",
        dataType: "json",
        async: false,
        success: function (result) {
            $.each(result, function (key, item) {
                $('#analyzerLog').hide();
                html += '<p>' + item + '</p>';
                setTimeout(updateProgress, 1, ++count, result.length, html);
                html = ''
            });

            $('#analyzerLog').show();
        },
        error: function (xhr, status, error) {
            $("#message").html("<strong>Can't analyze source code</strong>.<br/>" +
                xhr.responseText);
            return false;
        }
    });

    $('#detailedReportForm').submit(function () {
        $('#btnGetDetailedReport').empty();
        $('#btnGetDetailedReport').append('<i class="spinner-border spinner-border-sm"></i> Creating report');
        $("#btnGetDetailedReport").prop('disabled', true);
    });
});

function updateProgress(count, max, html) {
    if (count <= max) {
        percents = parseInt(count / (max / 100));
        var loadingStatus = document.getElementById("loadingStatus");
        loadingStatus.style.width = percents + "%";
        $("#loadingStatus").text(percents + '% completed');
        $("#analyzerLog").append(html);
    }
}
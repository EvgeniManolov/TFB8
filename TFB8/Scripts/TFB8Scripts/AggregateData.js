$(document).ready(function () {
    aggregateDataList();
});


function aggregateDataList() {
    $.ajax({
        url: '/api/aggregateData/',
        type: 'GET',
        dataType: 'json',
        success: function (aggregateData) {
            aggregateDataListSuccess(aggregateData);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function aggregateDataListSuccess(aggregateData) {
    $.each(aggregateData.TopStudents,
        function (index, student) {
            aggregateDataTopStudentAddRow(student);
        });

    $.each(aggregateData.NoScores,
        function (index, score) {
            aggregateDataNoScoreAddRow(score);
        });
}

// Add Discipline row to <table>
function aggregateDataTopStudentAddRow(student) {
    if ($("#topStudents tbody").length == 0) {
        $("#topStudents").append("<tbody></tbody>");
    }

    $("#topStudents tbody").append(
        topStudentsBuildTableRow(student));
}

function aggregateDataNoScoreAddRow(score) {
    if ($("#noScores tbody").length == 0) {
        $("#noScores").append("<tbody></tbody>");
    }

    $("#noScores tbody").append(
        noScoresBuildTableRow(score));
}

function topStudentsBuildTableRow(student) {
    var result = "<tr>" +
        "<td>" + student.SemesterName + "</td>" +
        "<td>" + student.StudentName + "</td>" +
        "<td>" + student.AverageScore + "</td>" +
        "</tr>";

    return result;
}


function noScoresBuildTableRow(score) {
    var result = "<tr>" +
        "<td>" + score.SemesterName + "</td>" +
        "<td>" + score.StudentName + "</td>" +
        "<td>" + score.DisciplinesName + "</td>" +
        "</tr>";

    return result;
}



// Handle exceptions from AJAX calls
function handleException(request, message, error) {
    var msg = "";

    msg += "Code: " + request.status + "\n";
    msg += "Text: " + request.responseText + "\n";
    if (request.responseJSON != null && request.responseJSON.Message !== undefined) {
        msg += "Message" + request.responseJSON.Message + "\n";
    }

    alert(msg);
}
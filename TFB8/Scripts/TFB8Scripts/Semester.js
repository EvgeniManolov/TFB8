$(document).ready(function () {
    semestersList();
});

var Semester = {
    SemesterId: 0,
    StartDate: "",
    EndDate: "",
    DisciplinesAsString: ""
}


function semestersList() {
    // Call Web API to get a list of Semesters
    $.ajax({
        url: '/api/Semester/',
        type: 'GET',
        dataType: 'json',
        success: function (Semesters) {
            semesterListSuccess(Semesters);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Display all Semesters returned from Web API call
function semesterListSuccess(semesters) {
    // Iterate over the collection of data
    $.each(semesters,
        function (index, semester) {
            // Add a row to the Semester table
            semesterAddRow(semester);
        });
}

// Add Semester row to <table>
function semesterAddRow(semester) {
    if ($("#semesterTable semester").length == 0) {
        $("#semesterTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#semesterTable tbody").append(
        semesterBuildTableRow(semester));
}

// Build a <tr> for a row of table data
function semesterBuildTableRow(semester) {
    var result = "<tr>" +
        "<td>" +
        "<button type='button' " +
        "onclick='semesterGet(this);' " +
        "class='btn btn-default' " +
        "data-id='" + semester.SemesterId + "'>" +
        "<i class='fa fa-pencil'></i>" +
        "</button>" +
        "</td>" +
        "<td>" + semester.Name + "</td>" +
        "<td>" + semester.StartDate + "</td>" +
        "<td>" + semester.EndDate + "</td>" +
        "<td>" + semester.DisciplinesAsString + "</td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='semesterDelete(this);' " +
        "class='btn btn-default' " +
        "data-id='" + semester.SemesterId + "'>" +
        "<span class='fa fa-remove' />" +
        "</button>" +
        "</td>" +
        "</tr>";



    return result;
}


// Handle exceptions from AJAX calls
function handleException(request, message, error) {
    var msg = "";

    msg += "Code: " + request.status + "\n";
    msg += "Text: " + request.statusText + "\n";
    if (request.responseJSON != null) {
        msg += "Message" + request.responseJSON.Message + "\n";
    }

    alert(msg);
}
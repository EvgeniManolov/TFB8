$(document).ready(function () {
    semestersList();
    loadDisciplinesDropdown();

});

var Semester = {
    SemesterId: 0,
    StartDate: "",
    EndDate: "",
    Name: "",
    Disciplines: []
}


function updateClick() {
    // Build semesters object from inputs
    semester = new Object();
    semester.SemesterId = $("#semesterid").val();
    semester.StartDate = $("#startdate").val();
    semester.EndDate = $("#enddate").val();
    semester.Name = $("#name").val();
    semester.Disciplines = [];

    var disciplinesIds = $('select#disciplinesDropdawn').val();

    for (var i = 0; i < disciplinesIds.length; i++) {
        var discipline = { DisciplineId: disciplinesIds[i] };
        semester.Disciplines.push(discipline);
    }


    if ($("#updateButton").text().trim() == "Add") {
        semesterAdd(semester);
    }
    else {
        semesterUpdate(semester);
    }
}

function semesterUpdate(semester) {
    var url = "/api/Semester/" + semester.SemesterId;

    // Call Web API to update semester
    $.ajax({
        url: url,
        type: 'PUT',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(semester),
        success: function (responseMessage) {
            semesterUpdateSuccess();
            success(responseMessage);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function loadDisciplinesDropdown() {
    // Call Web API to get a list of Semesters
    $.ajax({
        url: '/api/Discipline/',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            $.each(result, function (i) {
                $('#disciplinesDropdawn').append($('<option></option>').val(result[i].DisciplineId).html(result[i].DisciplineName));
            });
            $('option').mousedown(function (e) {
                e.preventDefault();
                $(this).prop('selected', !$(this).prop('selected'));
                return false;
            });
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}


function semesterGet(ctl) {
    // Get semester id from data- attribute
    var id = $(ctl).data("id");

    // Store semester id in hidden field
    $("#semesterid").val(id);

    // Call Web API to get a semester
    $.ajax({
        url: "/api/Semester/" + id,
        type: 'GET',
        dataType: 'json',
        success: function (semester) {
            semesterToFields(semester);

            // Change Update Button Text
            $("#updateButton").text("Update");
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function semesterToFields(semester) {
    $("#name").val(semester.Name);
    var startDate = createDate(semester.StartDate)
    var enddate = createDate(semester.EndDate)
    $("#startdate").val(startDate);
    $("#enddate").val(enddate);
    $('#disciplinesDropdawn option:selected').removeAttr('selected');
    var options = document.getElementById('disciplinesDropdawn').options;

    for (i = 0; i < options.length; i++) {
        for (r = 0; r < semester.Disciplines.length; r++) {
            var disciplineId = parseInt(semester.Disciplines[r].DisciplineId);
            var optiondId = parseInt(options[i].value);
            if (disciplineId === optiondId) {
                $('#disciplinesDropdawn > option[value="' + optiondId + '"]').attr("selected", "selected")
            }
        }
    }

}

function createDate(unformatedDate) {
    var newDate = new Date(unformatedDate);
    var day = newDate.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var year = newDate.getFullYear();
    var month = parseInt(newDate.getMonth()) + 1;
    if (month < 10) {
        month = "0" + month;
    }
    var date = year + "-" + month + "-" + day;
    return date;
}


function semesterAdd(semester) {
    // Call Web API to add a new Semester
    $.ajax({
        url: "/api/Semester",
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(semester),
        success: function (semester) {
            semesterAddSuccess(semester);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function semesterAddSuccess(Semester) {
    var row = document.getElementsByTagName('tbody')[0];
    row.parentNode.removeChild(row);

    semestersList();
    formClear();
}

function deleteRow() {
    row.parentNode.removeChild(row);
};

// Clear form fields
function formClear() {
    $("#name").val("");
    $("#startdate").val("");
    $("#enddate").val("");
    $('#disciplinesDropdawn option').prop('selected', false);
}

function success(data) {
    alert(data);
}
function error(data) {
    alert(data);
}

function semesterDelete(ctl) {
    var id = $(ctl).data("id");

    // Call Web API to delete a  discipline
    $.ajax({
        url: "/api/Semester/" + id,
        type: 'DELETE',
        success: function (responseMessage) {
            success(responseMessage)
            $(ctl).parents("tr").remove();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
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
    if ($("#semesterTable tbody").length == 0) {
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

function addClick() {
    formClear();
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
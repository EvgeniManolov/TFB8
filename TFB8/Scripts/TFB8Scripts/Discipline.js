$(document).ready(function () {
    disciplinesList();
});

var Discipline = {
    DisciplineId: 0,
    DisciplineName: "",
    ProfessorName: "",
}

function updateClick() {
    // Build Discipline object from inputs
    Discipline = new Object();
    Discipline.DisciplineId = $("#disciplineid").val();
    Discipline.DisciplineName = $("#disciplinename").val();
    Discipline.ProfessorName = $("#professorname").val();

    if ($("#updateButton").text().trim() == "Add") {
        disciplineAdd(Discipline);
    }
    else {
        disciplineUpdate(Discipline);
    }
}

function disciplineUpdate(discipline) {
    var url = "/api/Discipline/" + discipline.DisciplineId;

    // Call Web API to update discipline
    $.ajax({
        url: url,
        type: 'PUT',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(discipline),
        success: function (discipline) {
            disciplineUpdateSuccess(discipline);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function disciplineUpdateSuccess(discipline) {
    var row = document.getElementsByTagName('tbody')[0];
    row.parentNode.removeChild(row);

    disciplinesList();
    formClear();
}

function disciplineAdd(discipline) {

    if (discipline.DisciplineName === "") {
        error("Discipline name is requered!")
    } else if (discipline.ProfessorName === "") {
        error("Professor name is requered!")
    }

    else {


        // Call Web API to add a new discipline
        $.ajax({
            url: "/api/Discipline",
            type: 'POST',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(discipline),
            success: function (discipline) {
                disciplineAddSuccess(discipline);
                success("Successfully created discipine");
            },
            error: function (request, message, error) {
                handleException(request, message, error);
            }
        })
    };
}
function success(data) {
    alert(data);
}
function error(data) {
    alert(data);
}

function disciplineAddSuccess(discipline) {
    var row = document.getElementsByTagName('tbody')[0];
    row.parentNode.removeChild(row);

    disciplinesList();
    formClear();
}

function deleteRow() {
    row.parentNode.removeChild(row);
};

// Clear form fields
function formClear() {
    $("#disciplinename").val("");
    $("#professorname").val("");
}


function disciplineGet(ctl) {
    // Get discipline id from data- attribute
    var id = $(ctl).data("id");

    // Store discipline id in hidden field
    $("#disciplineid").val(id);

    // Call Web API to get a discipline
    $.ajax({
        url: "/api/Discipline/" + id,
        type: 'GET',
        dataType: 'json',
        success: function (discipline) {
            disciplineToFields(discipline);

            // Change Update Button Text
            $("#updateButton").text("Update");
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function disciplineToFields(discipline) {
    $("#disciplinename").val(discipline.DisciplineName);
    $("#professorname").val(discipline.ProfessorName);
}

function disciplinesList() {
    // Call Web API to get a list of Disciplines
    $.ajax({
        url: '/api/Discipline/',
        type: 'GET',
        dataType: 'json',
        success: function (disciplines) {
            disciplineListSuccess(disciplines);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Display all Disciplines returned from Web API call
function disciplineListSuccess(disciplines) {
    // Iterate over the collection of data
    $.each(disciplines,
        function (index, discipline) {
            // Add a row to the Discipline table
            disciplineAddRow(discipline);
        });
}

// Add Discipline row to <table>
function disciplineAddRow(discipline) {
    if ($("#disciplineTable tbody").length == 0) {
        $("#disciplineTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#disciplineTable tbody").append(
        disciplineBuildTableRow(discipline));
}

// Build a <tr> for a row of table data
function disciplineBuildTableRow(discipline) {
    var result = "<tr>" +
        "<td>" +
        "<button type='button' " +
        "onclick='disciplineGet(this);' " +
        "class='btn btn-default' " +
        "data-id='" + discipline.DisciplineId + "'>" +
        "<i class='fa fa-pencil'></i>" +
        "</button>" +
        "</td>" +
        "<td>" + discipline.DisciplineName + "</td>" +
        "<td>" + discipline.ProfessorName + "</td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='disciplineDelete(this);' " +
        "class='btn btn-default' " +
        "data-id='" + discipline.DisciplineId + "'>" +
        "<span class='fa fa-remove' />" +
        "</button>" +
        "</td>" +
        "</tr>";



    return result;
}

function addClick() {
    formClear();
}

function disciplineDelete(ctl) {
    var id = $(ctl).data("id");

    // Call Web API to delete a  discipline
    $.ajax({
        url: "/api/Discipline/" + id,
        type: 'DELETE',
        success: function (discipline) {
            success("Successfully deleted discipline!")
            $(ctl).parents("tr").remove();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Handle exceptions from AJAX calls
function handleException(request, message, error) {
    var msg = "";

    msg += "Code: " + request.status + "\n";
    msg += "Text: " + request.responseText + "\n";
    if (request.responseJSON != null) {
        msg += "Message" + request.responseJSON.Message + "\n";
    }

    alert(msg);
}
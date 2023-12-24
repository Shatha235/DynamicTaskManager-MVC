document.addEventListener('DOMContentLoaded', function () {
    // Function to show the full description
    function showFullDescription(element) {
        var fullDescription = element.getAttribute('data-fulltext');
        var shortDescription = fullDescription.length > 100 ? fullDescription.substring(0, 100) + "..." : fullDescription;
        var descriptionContainer = element.previousElementSibling;
        var isShowingMore = element.textContent.includes('Read Less');

        if (isShowingMore) {
            descriptionContainer.textContent = shortDescription;
            element.textContent = 'Read More';
        } else {
            descriptionContainer.textContent = fullDescription;
            element.textContent = 'Read Less';
        }
    }

    // Attach event listeners to 'Read More' links
    var readMoreLinks = document.querySelectorAll('.read-more');
    readMoreLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            showFullDescription(this);
        });
    });


    // Event listener for closing the modal when clicked outside
    window.onclick = function (event) {
        var modal = document.getElementById("deleteModal");
        if (event.target === modal) {
            closeModal();
        }
    };

    // Attach the closeModal function to the cancel button
    document.getElementById("cancelDelete").onclick = function () {
        closeModal();
    };



    // Initialize SortableJS for drag-and-drop functionality
    var statusGroups = document.querySelectorAll('.task-status-group');
    statusGroups.forEach(function (group) {
        new Sortable(group, {
            group: 'shared',
            onEnd: function (evt) {
                var itemEl = evt.item;
                var newStatus = evt.to.id.split('-')[2]; // New status group
                var taskId = itemEl.id.split('-')[1]; // Task ID

                updateTaskStatus(taskId, newStatus);
            }
        });
    });

    // Function to update task status
    function updateTaskStatus(taskId, newStatus) {
        console.log("updateTaskStatus called with taskId:", taskId, "and newStatus:", newStatus);

        $.ajax({
            url: '/Mytasks/EditStatus',
            type: 'POST',
            data: { id: taskId, status: newStatus },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Status updated successfully.',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: response.message,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function (xhr, status, error) {
                if (xhr.status === 404) {
                    Swal.fire({
                        title: 'Not Found!',
                        text: 'Task not found.',
                        icon: 'warning',
                        confirmButtonText: 'OK'
                    });
                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'An error occurred while updating the status: ' + error,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            }
        });
    }

});

// Function to show the delete modal
function showModal(taskId, taskName) {
    var modal = document.getElementById("deleteModal");
    var header = modal.querySelector('.modal-header h5');

    // Update the modal header to include the task name
    header.innerHTML = "Delete Confirmation - <span class='task-name'>" + taskName + "</span>";

    // Display the modal
    modal.style.display = "block";

    // Attach the event handler to the confirm button
    var confirmBtn = document.getElementById("confirmDelete");
    confirmBtn.onclick = function () {
        deleteTask(taskId);
    };
}

// Function to close the modal
function closeModal() {
    var modal = document.getElementById("deleteModal");
    modal.style.display = "none";
}

// AJAX function to delete the task
function deleteTask(taskId) {
    $.ajax({
        url: '/Mytasks/DeleteConfirmed/' + taskId,
        type: 'POST',
        data: { id: taskId },
        success: function (response) {
            closeModal();
            location.reload();
        },
        error: function (xhr, status, error) {
            console.error("Error deleting task:", xhr.responseText);
        }
    });
}



// Event listener for closing the modal when clicked outside
window.onclick = function (event) {
    var modal = document.getElementById("deleteModal");
    if (event.target === modal) {
        closeModal();
    }
};

// Attach the closeModal function to the cancel button
document.getElementById("cancelDelete").onclick = function () {
    closeModal();
};


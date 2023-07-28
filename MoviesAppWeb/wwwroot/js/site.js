//delete movie
function confirmDeleteMovie(itemId) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you can't get it back!",
        icon: "warning",
        buttons: ['Cancel', 'Yes'],
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: "/Movie/Delete/" + itemId,
                type: "GET",
                dataType: "json",
                success: function (response) {
                    if (response.success) {
                        swal(response.message)
                            .then((value) => {
                                window.location.reload();
                            });

                    } else {
                        toastr.error(response.message)
                    }
                },
                error: function () {
                    toastr.error('Something went wrong!');
                }
            });
        }
    });
} 

  
    ////////////////////////////////////////////////////////////////

    //delete moviecategory
    function confirmDelete(itemId) {
        swal({
            title: "Are you sure?",
            text: "Once deleted, you can't get it back!",
            icon: "warning",
            buttons: ['Cancel', 'Yes'],
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/MovieCategory/Delete/" + itemId,
                    type: "GET",
                    dataType: "json",
                    success: function (response) {
                        if (response.success) {
                            swal(response.message)
                                .then((value) => {
                                    window.location.reload();
                                });

                        } else {
                            toastr.error(response.message)
                        }
                    },
                    error: function () {
                        toastr.error('Something went wrong!');
                    }
                });
            }
        });
    }


//////////////////////////////////////////////////////

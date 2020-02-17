function AjaxPost(url,id,quantity) {
    $.ajax(
        {
            type: "POST",
            url: url,
            data: {
                foodItemId: id,
                quantity: quantity
            },
            success: (response) => {
                $("#cart").html(response);
                $("#cartModal").removeClass("fade").modal("show").on('hidden.bs.modal', function () {
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                });
            }, dataType: "html"
        });
}

$(".MultiSelect").children("option").mousedown(function (e) {
    e.preventDefault();
    $(this).prop("selected", !$(this).prop("selected"));
    return false;
});
$(document).ready(function () {
    const quantity = 1;

    $('.quantity-right-plus').click(function (event) {
        event.preventDefault();

        const quantity = parseInt($('#quantity').val());
        $('#quantity').val(quantity + 1);
    });

    $('.quantity-left-minus').click(function (event) {
        event.preventDefault();

        const quantity = parseInt($('#quantity').val());
        if (quantity > 1) {
            $('#quantity').val(quantity - 1);
        }
    });
});

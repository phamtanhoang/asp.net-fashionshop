
$(function () {
    $('.add-to-cart-btn').click(function () {
        var productId = $(this).data('product-id');
        $.ajax({
            url: "/Cart/AddToCart",
            data: {
                productId: productId, 
            },
            success: function (data) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Thêm thành công!!!',
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        });
    });
    $('.delete-item-cart').click(function () {
        Swal.fire({
            title: 'Bạn có muốn xóa sản phẩm khỏi giỏ hàng?',
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đồng ý'
        }).then((result) => {

            if (result.isConfirmed) {
                var productId = $(this).data('product-id');
                $.ajax({
                    url: "/Cart/DeleteItemToCart",
                    data: {
                        productId: productId,
                    },
                    success: function (data) {                       
                    }
                });
                $(this).closest('.row').next('hr').remove();
                $(this).closest('.row').remove();
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Xóa thành công!!!',
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        })

    });
});


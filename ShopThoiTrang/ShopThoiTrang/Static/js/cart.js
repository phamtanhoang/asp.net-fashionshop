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
                setTimeout(function () {
                    location.reload();
                }, 1500);
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
                setTimeout(function () {
                    location.reload();
                }, 1500);
            }
        })
    });
    $('.prod-quantity').on('change', function () {
        var productId = $(this).data('product-id');
        var quantity = $(this).val();
        // Gửi yêu cầu AJAX để cập nhật giỏ hàng trong session
        $.ajax({
            url: '/Cart/UpdateCart',
            type: 'POST',
            data: { productId: productId, quantity: quantity },
        });
        location.reload();
    });
    $('.delete-all-cart').click(function () {
        Swal.fire({
            title: 'Bạn có muốn xóa toàn bộ sản phẩm khỏi giỏ hàng?',
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đồng ý'
        }).then((result) => {

            if (result.isConfirmed) {
                $.ajax({
                    url: "/Cart/DeleteAllCart",
                });
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Xóa thành công!!!',
                    showConfirmButton: false,
                    timer: 1500
                })
                setTimeout(function () {
                    location.reload();
                }, 1500);
            }
        })
    });
    $('.btn-buy').click(function () {
        var address = $('.delivery-address')[0].value;
        var cart = $('.cart-null');
        if (address == "" || address == null) {
            Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Vui lòng nhập địa chỉ giao hàng!!!',
                showConfirmButton: false,
                timer: 1500
            })
        }
        else if (cart.length > 0) {

            Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Giỏ hàng đang rỗng!!!',
                showConfirmButton: false,
                timer: 1500
            })
        }
        else {
            Swal.fire({
                title: 'Bạn chắc chắn muốn đặt hàng?',
                text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Đồng ý'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Cart/Order",
                        data: {
                            address: address,
                        },
                    });
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Đặt hàng thành công!!!',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    setTimeout(function () {
                        location.reload();
                    }, 1500);
                }
            })
        }
    });
});


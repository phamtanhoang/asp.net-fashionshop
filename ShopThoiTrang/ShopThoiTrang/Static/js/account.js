$(document).ready(function () {
    // Lắng nghe sự kiện click vào thẻ <a> có id là "logout-link"
    $('#logout-link').click(function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Bạn có muốn đăng xuất?',
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/user/logout',
                });
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Đăng xuất thành công!!!',
                    showConfirmButton: false,
                    timer: 1500
                })
                setTimeout(function () {
                    location.reload();
                }, 1500);
            }
        })
    });
});

// Tạo đối tượng giỏ hàng
var cart = {
    items: [],
    addItem: function (product) {
        // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
        var found = false;
        for (var i = 0; i < this.items.length; i++) {
            if (this.items[i].productId == product.productId) {
                this.items[i].quantity++;
                found = true;
                break;
            }
        }

        // Nếu chưa có trong giỏ hàng thì thêm sản phẩm mới vào
        if (!found) {
            this.items.push({
                productId: product.productId,
                productName: product.productName,
                unitPrice: product.unitPrice,
                quantity: 1
            });
        }

        // Lưu đối tượng giỏ hàng vào Session Storage
        sessionStorage.setItem('cart', JSON.stringify(this));
    }
};

function getCartFromSessionStrorage() {
    var cartList = $('.cart-list');
    cartList.empty(); // Xóa toàn bộ sản phẩm cũ trước khi thêm sản phẩm mới
    var cartItems = JSON.parse(sessionStorage.getItem('cartItems')) || [];
    for (var i = 0; i < cartItems.length; i++) {
        var cartItem = cartItems[i];
        var productWidget = $('<div class="product-widget"></div>');

        // Thêm ảnh sản phẩm vào widget
        var productImg = $('<div class="product-img"><img src="' + cartItem.productImage + '" alt=""></div>');
        productWidget.append(productImg);

        // Thêm thông tin sản phẩm vào widget
        var productBody = $('<div class="product-body"></div>');
        var productName = $('<h3 class="product-name"><a href="/Shop/Details/' + cartItem.productId + '">' + cartItem.productName + '</a></h3>');
        var productPrice = $('<h4 class="product-price" style="color:red"><i style="color:black">' + cartItem.quantity + 'x</i> ' + parseFloat(cartItem.unitPrice).toLocaleString('vi-VN').toString() + ' VND</h4>');
        productBody.append(productName).append(productPrice);
        productWidget.append(productBody);

        // Thêm nút xóa sản phẩm
        var deleteBtn = $('<button class="delete"><i class="fa fa-close" style="width:100%"></i></button>');
        productWidget.append(deleteBtn);

        cartList.append(productWidget);
    }
    var total = 0;
    for (var i = 0; i < cartItems.length; i++) {
        total += cartItems[i].quantity * cartItems[i].unitPrice;
    }
    // Hiển thị tổng giá trị của giỏ hàng trên giao diện
    $('#cart-total').text('Tổng tiền: ' + parseFloat(total).toLocaleString('vi-VN').toString() + ' VND');

    var sumprod = 0;
    for (var i = 0; i < cartItems.length; i++) {
        sumprod += cartItems[i].quantity;
    }
    $('.qty').text(sumprod);


    // Thêm sự kiện click vào nút xóa sản phẩm
    $('.delete').click(function () {
        if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng không?")) {
            // Lấy ra vị trí của sản phẩm trong mảng cartItems dựa vào vị trí của nút xóa
            var index = $(this).closest('.product-widget').index();
            // Lấy giỏ hàng từ sessionStorage
            var cartItems = JSON.parse(sessionStorage.getItem('cartItems')) || [];
            // Xóa sản phẩm khỏi giỏ hàng
            cartItems.splice(index, 1);
            // Lưu giỏ hàng mới vào sessionStorage
            sessionStorage.setItem('cartItems', JSON.stringify(cartItems));
            // Cập nhật lại danh sách sản phẩm trên giao diện
            getCartFromSessionStrorage();
        }
    });
}
$(document).ready(function () {
    $('.add-to-cart-btn').click(function () {
        var productId = $(this).data('product-id');
        var productImage = $(this).data('product-image');
        var productName = $(this).data('product-name');
        var productPrice = $(this).data('product-price');

        // Lấy giỏ hàng từ sessionStorage
        var cartItems = JSON.parse(sessionStorage.getItem('cartItems')) || [];

        // Tìm sản phẩm trong giỏ hàng
        var foundIndex = -1;
        for (var i = 0; i < cartItems.length; i++) {
            if (cartItems[i].productId == productId) {
                foundIndex = i;
                break;
            }
        }

        // Nếu sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng lên 1
        if (foundIndex != -1) {
            cartItems[foundIndex].quantity++;
        }
        // Ngược lại, thêm sản phẩm vào giỏ hàng với số lượng là 1
        else {
            cartItems.push({
                productId: productId,
                productName: productName,
                productImage: productImage,
                unitPrice: productPrice,
                quantity: 1
            });
        }

        // Lưu giỏ hàng vào sessionStorage
        sessionStorage.setItem('cartItems', JSON.stringify(cartItems));

        // Hiển thị thông báo thêm sản phẩm vào giỏ hàng thành công
        alert('Thêm sản phẩm vào giỏ hàng thành công!');

        getCartFromSessionStrorage();
    });

    getCartFromSessionStrorage();

});

function DeleteAllCart() {
    if (sessionStorage.length === 0) {
        alert('Giỏ hàng đang rỗng!');
        return
    }
    if (confirm("Bạn có chắc chắn muốn xóa tất cả giỏ hàng không?")) {
        sessionStorage.clear();
        getCartFromSessionStrorage();
    }
}

var cartItemsJson = window.sessionStorage.getItem('CartItems');
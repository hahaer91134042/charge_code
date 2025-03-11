
new Mmenu(
    document.querySelector('#menu'),
    {
        extensions: ['theme-dark'],
        setSelected: true,
        counters: true,        
        columns: true,
        dropdown: {
            drop: true,
            event:'hover click'
        },
        //searchfield: {
        //    placeholder: 'Search menu items',
        //},
        //iconbar: {
        //    use: '(min-width: 450px)',
        //    top: [
        //        '<a href="#/"><span class="fa fa-home"></span></a>',
        //    ],
        //    bottom: [
        //        '<a href="#/"><span class="fa fa-twitter"></span></a>',
        //        '<a href="#/"><span class="fa fa-facebook"></span></a>',
        //        '<a href="#/"><span class="fa fa-youtube"></span></a>',
        //    ],
        //},
        //sidebar: {
        //    collapsed: {
        //        use: false,
        //        hideNavbar: false,
        //    },
        //    expanded: {
        //        use: '(min-width: 230px)',
        //        initial:'remember'
        //    },
        //},
        //navbar: {
        //    title:""
        //},
        navbars: [
            {
                position: 'top',
                content: [
                    `<div id="logo"><a href="/Home/Main">LOGO</a></div>`
                ]
            },
            {
                use: false,
                content: ['title'],
            },
            {
                content: ['searchfield'],
            },
            //{
            //    type: 'tabs',
            //    content: [
            //        '<a href="#panel-menu"><i class="fa fa-bars"></i> <span>Menu</span></a>',
            //        '<a href="#panel-account"><i class="fa fa-user"></i> <span>Account</span></a>',
            //        '<a href="#panel-cart"><i class="fa fa-shopping-cart"></i> <span>Cart</span></a>',
            //    ],
            //},
            {
                content: ['prev', 'breadcrumbs','close'],
            },
            {
                position: 'bottom',
                content: [
                    `<div id="menu_footer">Copyright ©${moment().year()} EKI All right Reserved.Designed by  <a href="http://www.eki.com.tw/tc/index.html" target="_blank">EKI</a></div>`,
                ],
            },
        ],
    },
    {
        //searchfield: {
        //    clear: true,
        //},
        navbars: {
            breadcrumbs: {
                removeFirst: true,
            },
        },
        dropdown: {
            
            width: {
                max:750
            },
            offset: {

                button: {
                    x: 5
                }
            }
        },
    }
);

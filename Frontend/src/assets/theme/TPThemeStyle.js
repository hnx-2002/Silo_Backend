var TPThemeStyle = {
    common: {
        scrollbarBorderRadius: '4px',
        borderRadiusSmall: '4px',
        borderRadius: '4px',
        heightMedium: '32px',
    },
    Button: {
        borderPrimary: '0px',
        borderHoverPrimary: '0px',
        borderPressedPrimary: '0px',
        borderFocusPrimary: 'none',
        borderDisabledPrimary: '0px',
        paddingMedium: '0 16px', //按钮默认Medium，响应规定
        fontSizeMedium: '12px', //按钮默认Medium，响应规定
        borderRadiusMedium: '3px', //按钮默认Medium，响应规定
        iconMarginMedium: '8px', //按钮默认Medium，响应规定
        iconSizeMedium: '16px', //按钮默认Medium，响应规定
    },
    Card: {
        paddingMedium: '14px 14px 14px 14px',
        titleFontSizeMedium: '16px', //响应规定
        borderRadius: '4px',
        titleFontWeight: '500',
        closeBorderRadius: '4px',
        fontSizeSmall: '14px',
        fontSizeMedium: '14px',
        fontSizeLarge: '14px',
        borderRadius: '2px',
    },
    Layout: {},
    Menu: {
        fontSize: '14px',
        // itemHeight: '38px', //由于左右布局为38px,上下布局为54px，因此此处不做限制
        borderRadius: '0px',
    },
    IconWrapper: {},
    Table: {},
    Checkbox: {
        sizeMedium: '14px',
        borderRadius: '2px',
    },
    DataTable: {
        tdPaddingMedium: '10px',
        tdPaddingLarge: '10px',
        fontSizeSmall: '12px', //字体大小 默认14px
        fontSizeMedium: '12px', //字体大小 默认14px
        fontSizeLarge: '14px', //字体大小 默认15px
        thFontWeight: '600', //表头字体weight 默认500
        //thPadding 表头格大小
        //sorterSize 表头排序图标大小
    },
    Input: {
        heightMedium: '32px',
        paddingMedium: '0 9px 0 16px', //响应规定
        borderRadius: '3px',
    },
    Icon: {},
    Drawer: {
        footerPadding: '18px 24px',
    },
    List: {},
    Breadcrumb: {
        fontSize: '12px',
    },
    Tabs: {
        tabBorderRadius: '0px',
    },
    Tree: {
        fontSize: '12px',
        nodeBorderRadius: '0px',
    },
    Tag: {
        heightMedium: '26px', //响应规定
        boderRadius: '2px', //响应规定
        fontSizeMedium: '12px', //响应规定
        closeIconSizeMedium: '9px',
        closeSizeMedium: '9px',
    },
    Timeline: {
        iconSizeMedium: '11px', //响应规定
        titleFontSizeMedium: '14px', //规定同默认值
        titleMarginMedium: '0 0 12px 0', //响应规定
    },
    Collapse: {
        titleFontSize: '12px',
    },
    Radio: {
        radioSizeMedium: '14px', // 响应规定
        fontSizeMedium: '12px', //响应规定
    },
    Switch: {
        buttonHeightMedium: '9px',
        railHeightMedium: '13px',

        buttonWidthMedium: '9px',
        railWidthMedium: '22px',
    },
    Message: {
        iconSize: '22px',
        fontSize: '16px',
        borderRadius: '2px',
    },
    Notification: {
        closeIconSize: '12px',
        padding: '18px 18px 20px 18px',
        titleFontSize: '14px',
        fontSize: '12px',
    },
    Tooltip: {
        padding: '14px',
        borderRadius: '2px',
    },
    Progress: {
        railHeight: '10px',
    },
};

export default TPThemeStyle;

import Icon_Num1 from '@common/icon/Icon_1.vue';
import Icon_Num2 from '@common/icon/Icon_2.vue';
import Icon_Num3 from '@common/icon/Icon_3.vue';
import { make_icon, make_divWithStyle, make_h_span } from '@utility/NaiveHComponent';
import type { MenuItem } from '../../types/MenuItem.d.ts';

//首页对象
export function getMenuHome(text: string): MenuItem {
  return {
    code: 'Home',
    label: text,
    path: 'Home',
    alias: text,
  };
}

//处理菜单图标
export function executeMenuIcon(val: MenuItem) {
  let lvl = val.path?.split('/') || [];
  if (lvl.length == 1) return make_icon(Icon_Num1);
  if (lvl.length == 2) return make_icon(Icon_Num2);
  if (lvl.length == 3) return make_icon(Icon_Num3);
}

//处理菜单文本
export function executeMenuLabel(text: string) {
  return make_divWithStyle('', {}, '', [make_h_span(text)]);
  //let lvl = val.menuPath.split('/');
  //if (lvl.length == 1) {
  //    return make_divWithStyle(
  //        '',
  //        {
  //            fontSize: '14px',
  //            fontWeight: 'bold',
  //        },
  //        '',
  //        text
  //    );
  //} else {
  //    return make_divWithStyle('', { fontSize: '12px' }, '', translate.t(val.label));
  //}
}

//删除Menu中Children为null的键值对
export function killMenuChildren(inMenus: MenuItem[]) {
  for (let menu of inMenus) {
    if (menu.children == null) {
      delete menu.children;
    } else {
      killMenuChildren(menu.children);
    }
  }
}

//通过给定的key，获取对应的菜单对象
export function searchMenuByKey(inMenus: MenuItem[], key: string): MenuItem {
  currentMenuObj = {} as MenuItem;
  findMenuByKey(inMenus, key);
  return currentMenuObj;
}

//给定菜单key，检索菜单
let currentMenuObj: MenuItem = {} as MenuItem;
function findMenuByKey(targetMenus: MenuItem[], inKey: string) {
  for (let targetMenu of targetMenus) {
    if (targetMenu.code == inKey) {
      currentMenuObj = targetMenu;
    } else {
      if (targetMenu.children && targetMenu.children.length > 0) {
        findMenuByKey(targetMenu.children, inKey);
      }
    }
  }
}

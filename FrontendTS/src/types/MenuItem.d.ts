// 基础菜单项接口 - 包含所有可能的属性
export interface MenuItem {
  code?: string;
  key?: string;
  label: string;
  path?: string;
  alias?: string;
  children?: MenuItem[];
}

export interface Menu {
    id: number;
    controller: string;
    label: string;
    icon: string;
    url: string;
    routerLink: string;
    visible: boolean;
    separator: boolean;
    target: string;
    badge: string;
    title: string;
    class: string;
    seq: number;
    parentId: number | null;
    items: Menu[];
}

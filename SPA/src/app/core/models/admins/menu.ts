export interface Menu {
    id: string;
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
    parentId: string | null;
    items: Menu[];
}

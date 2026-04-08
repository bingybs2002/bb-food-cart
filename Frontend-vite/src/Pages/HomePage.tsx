/*import {
  Menubar,
  MenubarContent,
  MenubarGroup,
  MenubarItem,
  MenubarMenu,
  MenubarSeparator,
  MenubarShortcut,
  MenubarTrigger,
} from "@/components/ui/menubar"


    export default function Hub() {
        return(
            <div>
                <Menubar>
                <MenubarMenu>
                    <MenubarTrigger>File</MenubarTrigger>
                    <MenubarContent>
                    <MenubarGroup>
                        <MenubarItem>
                        New Tab <MenubarShortcut>⌘T</MenubarShortcut>
                        </MenubarItem>
                        <MenubarItem>New Window</MenubarItem>
                    </MenubarGroup>
                    <MenubarSeparator />
                    <MenubarGroup>
                        <MenubarItem>Share</MenubarItem>
                        <MenubarItem>Print</MenubarItem>
                    </MenubarGroup>
                    </MenubarContent>
                </MenubarMenu>
                </Menubar>
                <h1> Hi</h1>
            </div>
        )
    }
        */

import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"

export default function HomePage() {
  return (
    <SidebarProvider>
      <AppSidebar />
      <main>

      </main>
    </SidebarProvider>
  )
}
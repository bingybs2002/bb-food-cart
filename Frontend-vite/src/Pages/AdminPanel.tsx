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
import { Button } from "@/components/ui/button"
import { useState } from "react";
import { Separator } from "@/components/ui/separator";

export default function HomePage() {
  const [showImage, setShowImage] = useState(false)

  return (
    <SidebarProvider>
      <AppSidebar />

      <main className="p-6">
        <Button onClick={() => setShowImage(!showImage)}>
          Show Location
        </Button>

        {/* Only show image AFTER click */}
        {showImage && (
          <img
            src="/location.png"
            alt="location"
            className="mt-4 w-full max-w-md"
          />
        )}
      </main>
    </SidebarProvider>
  )
}
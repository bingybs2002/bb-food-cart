import { AppSidebar } from "@/components/app-sidebar";
import { Sidebar, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";

export default function GachaAddItem(){
    return(
        <SidebarProvider>
            <AppSidebar/>
            <main className="relative w-full space-y-6 p-6">
                <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB-Food | Add Gacha Item</h1>
                <div className="container mx-auto py-10">

                </div>
            <SidebarTrigger/>
            </main>
        </SidebarProvider>
    )
}

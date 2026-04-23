import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"



export default function EditItem(){
  return(
    <SidebarProvider>
      <AppSidebar/>
      <main className="relative w-full space-y-6 p-6">
        <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">
          Hello
          </h1>
        <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}
import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { DataTable } from "./ViewMenu/data-table"
import { columns, type MenuItems } from "./ViewMenu/columns"
import { useEffect, useState } from "react"
import {
  Card,
  CardAction,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

export default function ViewMenu(){
  const [Menu, SetMenu] = useState<MenuItems[]>([])

  useEffect(()=>{
    fetch(`https://localhost:63196/itemAdmin/Read`)
    .then((res)=>res.json())
    .then((data: MenuItems[])=>SetMenu(data))
  })
  return(
    <SidebarProvider>
      <AppSidebar/>
      <main className="relative w-full space-y-6 p-6">
        <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB Food Cart | View Menu</h1>
        <div className="container mx-auto py-10">
          <DataTable columns={columns} data={Menu}/>
        </div>
        <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}
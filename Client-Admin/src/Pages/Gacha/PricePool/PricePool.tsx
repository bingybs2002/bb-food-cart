import { AppSidebar } from "@/components/app-sidebar";
import { Sidebar, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import {columns, type GachaItems } from "./columns";
import { DataTable } from "./data-table";
import { useEffect, useState } from "react";

const API = import.meta.env.VITE_API_BASE_URL


export default function GachaPricePool(){

  const [items, setItems] = useState<GachaItems[]>([])

  useEffect(()=>{
    fetch(`${API}/Gacha/all-items`)
    .then((res)=>res.json())
    .then((data: GachaItems[])=>setItems(data))
  }, [])

  return(
      <SidebarProvider>
          <AppSidebar/>
          <main className="relative w-full space-y-6 p-6">

            <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB-Food | Price Pool</h1>
            <div className="container mx-auto py-10">
            </div>
            <h2>Gacha Items are listed below:</h2>
            <DataTable columns={columns} data={items} />

          <SidebarTrigger/>

          </main>
      </SidebarProvider>
  )
}


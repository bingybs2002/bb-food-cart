import { AppSidebar } from "@/components/app-sidebar";
import { Sidebar, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import { useState } from "react";
import {columns, type GachaItem} from "./columns"
import { DataTable } from "./data-table"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"

const API = import.meta.env.VITE_API_BASE_URL

export default function Gacha(){

    const [gacha, setGacha] = useState<GachaItem[]>([])

    const loadGachaItem = async () => {
    }

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

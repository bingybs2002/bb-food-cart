import { SidebarProvider,SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"
import { Sidebar } from "lucide-react"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Field, FieldDescription, FieldLabel } from "@/components/ui/field"
import { useState } from "react"
import { data } from "react-router-dom"

export default function Utilities(){
    const [allergies, setAllergies] = useState("")
    const [allergiesResult, setAllergiesResult] = useState("")
    const getAllergies = async() => {  
        const res = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/Allergies/${allergies}`)
        const data = await res.text()
        setAllergiesResult(data)
    }
    return(
        <SidebarProvider>
            <AppSidebar/>
            <main className="w-full h-full flex">
                <div className=" items-center">
                <Field className="justify-center">
                    <FieldLabel htmlFor="input-allergies">Allergies</FieldLabel>
                    <FieldDescription>
                        Enter the number to see what this number converts to in allergies.
                    </FieldDescription>
                    <input 
                    id="input-allergies" 
                    placeholder="Enter a number"
                    value={allergies}
                    onChange={(e) => setAllergies(e.target.value)}
                    />
                </Field>
                <Button onClick={getAllergies}/>
                <p>Allergies: <br/>{allergiesResult}</p>
                </div>
            </main>
        </SidebarProvider>
    )     
}
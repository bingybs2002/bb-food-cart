import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"
import { Button } from "@/components/ui/button"
import { useState } from "react";
import { Separator } from "@/components/ui/separator";
import { Calendar } from "@/components/ui/calendar";
import React from "react";

export default function HomePage() {
  const [showImage, setShowImage] = useState(false)
  const [date, setDate] = React.useState<Date | undefined>(new Date())

  return (
    <SidebarProvider>
      <AppSidebar />

      <main>
        <div className="flex items-center justify-between gap-4 px-4 py-2 border-b">
          <Button onClick={() => setShowImage(!showImage)}>
            Show Map
          </Button>

          {showImage && (
            <img
              src="/location.png"
              alt="location"
              className="w-40 max-w-md"
            />
          )}
        </div>

        <div flex-col gap-4 p-4>
            <Calendar
              mode="single"
              selected={date}
              onSelect={setDate}
              className="rounded-lg border"
            />
        </div>


      </main>
    </SidebarProvider>
  )
}
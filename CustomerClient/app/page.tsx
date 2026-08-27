import { Button } from "@/components/ui/button"
import LoginForm from "@/components/login-form"

export default function Page() {
  return (
    <div className="flex min-h-svh flex-col items-center justify-center gap-6 bg-background p-6">
      <div className="w-full max-w-sm"> 
        <LoginForm/> 
      </div>
    </div>
  )
}

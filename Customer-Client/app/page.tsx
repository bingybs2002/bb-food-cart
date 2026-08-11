import {LoginForm} from "@/components/login-form";

export default function Page(){
  const accessToken = localStorage.getItem("accessToken");
  const expiredTime = localStorage.getItem("expiredTime");
  const isExpired = !expiredTime || new Date(expiredTime) < new Date()
  if(accessToken && !isExpired){
    return <CustomerPanel/>
  }
  if(accessToken && isExpired){
    localStorage.clear();
  }
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <LoginForm />
      </div>
    </div>
  )
}
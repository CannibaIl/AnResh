export default async function({$auth, redirect}) {
    if(!$auth.loggedIn) {
        redirect('/login')
    }
    else if($auth.user.role !== 'Admin') {
        redirect('/')
    }
}
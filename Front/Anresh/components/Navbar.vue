<template>
  <b-navbar
    class="navbar fixed-top d-flex justify-content-end"
    toggleable="lg"
    type="dark"
  >
    <nuxt-link exact no-prefetch class="navbar-brand" to="/">
      <img src="~/assets/images/logo.svg" alt="" />
    </nuxt-link>

    <b-navbar-toggle target="nav-collapse">
      <svg width="50" height="50" viewBox="0 0 100 100">
        <path
          class="line line1"
          d="M 20,29.000046 H 80.000231 C 80.000231,29.000046 94.498839,28.817352 94.532987,66.711331 94.543142,77.980673 90.966081,81.670246 85.259173,81.668997 79.552261,81.667751 75.000211,74.999942 75.000211,74.999942 L 25.000021,25.000058"
        />
        <path class="line line2" d="M 20,50 H 80" />
        <path
          class="line line3"
          d="M 20,70.999954 H 80.000231 C 80.000231,70.999954 94.498839,71.182648 94.532987,33.288669 94.543142,22.019327 90.966081,18.329754 85.259173,18.331003 79.552261,18.332249 75.000211,25.000058 75.000211,25.000058 L 25.000021,74.999942"
        />
      </svg>
    </b-navbar-toggle>

    <!-- <b-button v-if="$auth.loggedIn" @click="$auth.logout()" variant="danger">logout</b-button> -->

    <div class="d-none d-lg-flex align-items-center ml-3" style="color: #fff">
      <b-avatar variant="secondary" text="JC"></b-avatar>
      <b-dropdown
      right
      :text="userName"
      class="mx-3"
    >
      <b-dropdown-item href="#">Action</b-dropdown-item>
      <b-dropdown-item href="#">Another action</b-dropdown-item>
      <b-dropdown-item @click="$auth.logout()">
        <fa icon="sign-out-alt"/>
        Sign out</b-dropdown-item>
    </b-dropdown>
    </div>

    <b-collapse id="nav-collapse" class="col-lg-2 navbar-container" is-nav>
      <ul>
        <li class="d-lg-none mb-5">
          <div style="color: #fff">
            <b-avatar class="mb-2" variant="secondary" text="JC"></b-avatar>
            <span class="d-block">{{userName}}</span>
          </div>
        </li>

        <li class="nav-item">
          <nuxt-link
            exact
            no-prefetch
            active-class="active"
            class="nav-link"
            to="/"
          >
            <fa class="nav-link__ico d-none d-lg-inline-flex" icon="home" style="color: #fff" />
            Home</nuxt-link
          >
        </li>
        <li class="nav-item">
          <nuxt-link active-class="active" class="nav-link" to="/employees">
            <fa class="nav-link__ico d-none d-lg-inline-flex" icon="users" style="color: #fff" />
            Employees</nuxt-link
          >
        </li>
        <li class="nav-item">
          <nuxt-link active-class="active" class="nav-link" to="/departments">
            <fa class="nav-link__ico d-none d-lg-inline-flex" icon="building" style="color: #fff" />
            Departments</nuxt-link
          >
        </li>
        <li class="nav-item">
          <nuxt-link v-if="$auth.user.role === 'Admin'" active-class="active" class="nav-link" to="/skills">
            <fa class="nav-link__ico d-none d-lg-inline-flex" icon="atom" style="color: #fff" />
            Skills</nuxt-link
          >
        </li>
        <li class="nav-item d-lg-none">
          <button class="nav-link" @click="$auth.logout()" style="width: 100%">Sign out</button>
        </li>
      </ul>
    </b-collapse>
  </b-navbar>
</template>

<script>
export default {
  computed: {
    userName() {
      return this.$auth.user === null ? "" : this.$getFullName(this.$auth.user)
    }
  }
};
</script>
<style lang="scss" scoped>
$dark-color: #131524;
$dark-hover-color: #242633;
$light-color: #e5e5e5;

.navbar {
  background-color: $dark-color;
  height: 72px;
  .navbar-brand {
    position: absolute;
    left: 0;
    top: 0;
    width: 300px;
    z-index: 10;
    img {
      width: 100%;
      transition: 0.2s;
      &:hover {
        width: 105%;
      }
    }
  }
  @supports ((-webkit-backdrop-filter: none) or (backdrop-filter: none)) {
  .navbar-container {
    background-color:  #131524d8!important;
    -webkit-backdrop-filter: blur(10px);
    backdrop-filter: blur(10px);
    .nav-item {
        
        :hover {
          background-color: #ffffff09!important;
        }
      }
  }
}
  .navbar-container {
    position: absolute;
    background-color: $dark-color;
    left: 0;
    top: 72px;
    height: calc(100vh - 72px);

    ul {
      position: absolute;
      padding-top: 100px;
      top: 0;
      right: 0;
      width: 110%;
      list-style: none;

      .nav-item {
        margin: 10px 0;
        .nav-link {
          padding: 0 0 0 10px;
          margin: 0;
          height: 32px;
          line-height: 32px;
          border-top-left-radius: 5px;
          border-bottom-left-radius: 5px;
          align-items: center;
          color: $light-color;
          transition: 0.2s;
        }
        :hover {
          background-color: $dark-hover-color;
          margin-left: 5px;
        }
        .active {
          color: $dark-color;
          & {
            background-color: $light-color;
            &:hover {
              background-color: #fff!important;
            }
          }
          .nav-link__ico {
            color: $dark-color !important;
          }
        }
      }
    }
  }
}
@media (max-width: 1400px) {
  .navbar-brand {
    width: 250px !important;
  }
  .navbar-container {
    ul {
      padding-top: 70px!important;
    }
  }
}

@media (max-width: 992px) {
  .navbar-brand {
    width: 120px !important;
  }
  .navbar-container {
    text-align: center;
    ul {
      padding: 0;
      width: 100%;
      .nav-link {
        padding: 0 0 0 10px!important;
        border-top-left-radius: 0 !important;
        border-bottom-left-radius: 0 !important;
      }
      :hover {
        margin-left: 0 !important;
      }
    }
  }
}

.line {
  fill: none;
  stroke: #fff;
  stroke-width: 6;
  transition: stroke-dasharray .3s cubic-bezier(0.4, 0, 0.2, 1),
  stroke-dashoffset .3s cubic-bezier(0.4, 0, 0.2, 1);
 
}
.collapsed {
  border: none;
   &:hover {
     .line {
       stroke: rgb(192, 192, 192);
     }
  }
  .line1 {
    stroke-dasharray: 60 207;
    stroke-width: 6;
  }
  .line2 {
    stroke-dasharray: 60 60;
    stroke-width: 6;
  }
  .line3 {
    stroke-dasharray: 60 207;
    stroke-width: 6;
  }
}

.not-collapsed {
  border: none;
  .line {
    stroke: #ee2b45;
  }
  .line1 {
    stroke-dasharray: 90 207;
    stroke-dashoffset: -134;
    stroke-width: 6;
  }
  .line2 {
    stroke-dasharray: 1 60;
    stroke-dashoffset: -30;
    stroke-width: 6;
  }
  .line3 {
    stroke-dasharray: 90 207;
    stroke-dashoffset: -134;
    stroke-width: 6;
  }
}
</style>


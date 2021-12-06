<template>
  <section class="col-lg-12 d-flex justify-content-end" style="background-color: #131524">
    <Navbar />
  
    <main class="col-lg-10 p-0 corner" >
      <div class="container-fluid">
        <b-breadcrumb nuxt-link-active :items="crumbs"></b-breadcrumb>
        <notifications position="bottom right" animation-type="velocity"/>
        <nuxt />
      </div>
    </main>
  </section>
</template>

<script>
import Navbar from '@/components/Navbar'

export default {
  middleware: 'auth',
  components: {
    Navbar
  },
  computed: {
    crumbs() {
      const fullPath = this.$route.fullPath
      const params = fullPath.startsWith('/')
        ? fullPath.substring(1).split('/')
        : fullPath.split('/')
      const crumbs = [{
          text: 'anresh',
          to: '/',
          }]
      let path = ''

      params.forEach((param, index) => {
        path = `${path}/${param}`
        const match = this.$router.match(path)

        crumbs.push({
          text: match.path.substring(match.path.lastIndexOf('/') + 1),
          to: match.fullPath,
          })
      })
      return crumbs
    },
  },
}
</script>

<style lang="scss" scoped>
  section {
    padding: 0;
    .corner {
      border-top-left-radius: 40px;
    }
    main {
      background-color: #E5E5E5;
      margin-top: 72px;
      height: calc(100vh - 72px);
      overflow-y: scroll;
      transition-property: width, height, padding;
      transition-duration: .1s;
      .container-fluid {
        margin-top: 30px;
        padding-left: 30px;
        .breadcrumb {
          padding: 0;
          margin: 0;
          background-color: rgba(255, 255, 255, 0);
          .breadcrumb-item {
            a {
              text-decoration: none;
              color: #000;
              &:hover {
                color: rgb(68, 68, 68);
              }
            }
            .active {
              color: #000;
            }
          }
        }
      }
    }
  }
  @media (max-width: 992px) {
    .corner {
      border-top-left-radius: 5px!important;
    }
    .container-fluid {
        padding-left: 15px!important;
    }
  }
</style>

<template>
  <section>
    wait
  </section>
</template>

<script>
export default {
  layout: 'empty',
  async mounted() {
    await this.$axios.get(`/api/user/confirm?token=${this.$route.params.token}`)
    .then((d) => {
          this.$router.replace('/login')
        })
        .catch((error) => {
          console.log(error)
        });
  }
  
}
</script>
<template>
  <section>
    <img class="empty-logo" src="~/assets/images/logo.svg" alt="" />
    <nuxt />
    <notifications position="bottom right" animation-type="velocity"/>
  </section>
</template>

<style lang="scss" scoped>
  .empty-logo {
    position: absolute;
    width: 30vw;
  }
</style>
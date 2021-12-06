<template>
  <section>
    <h1>Skills</h1>
    <Skill />
  </section>
</template>

<script>
import Skill from '~/components/skill.vue'
export default {
  middleware: 'auth-admin',
    components: { Skill },
};
</script>

<style scoped>
</style>

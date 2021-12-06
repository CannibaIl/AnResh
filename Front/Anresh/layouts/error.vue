<template>
  <section class="oopss">
    <div class="error-text">
      <img
        class="smile"
        src="https://cdn.rawgit.com/ahmedhosna95/upload/1731955f/sad404.svg"
        alt="404"
      />
      <span>Error 404</span>
      <p class="p-a">route {{ $route.path }} not found!</p>
      <nuxt-link class="go-home" to="/">
        <fa class="ico" icon="home" />
        <p>go to home</p>
      </nuxt-link>
    </div>
  </section>
</template>

<style lang="scss" scoped>
.oopss {
  width: 100%;
  height: 100%;
  line-height: 1.5em;
  z-index: 9999;
  .error-text {
    font-size: 40px;
    display: flex;
    flex-direction: column;
    align-items: center;
    color: #000;
    .smile {
      margin: 85px auto 20px;
      height: 342px;
    }
    span {
      position: relative;
      font-size: 3.3em;
      font-weight: 900;
      margin-bottom: 50px;
    }
    p {
      &.p-a {
        font-size: 19px;
        margin: 30px 0 15px 0;
      }
      &.p-b {
        font-size: 15px;
      }
    }
    .go-home {
      text-decoration: none;
      display: flex;
      align-items: center;
      font-size: 30px;
      font-weight: 400;
      color: #0062cc;
      transition: .2s;
      .ico {
        margin-right: 10px;
      }
      &:hover {
        font-weight: 500;
        color: #0d4b8d;
      }
    }
  }
}

@media (max-width: 640px) {
  .error-text {
    span {
     font-size: 18vw!important;
     margin-bottom: 5vw!important;
    }
  }
}
@media (max-width: 342px) {
  .error-text {
    img {
      width: calc(100vw - 15%) ;
    }
  }
}
</style>
pipeline {
  agent any
  stages {
    stage('CI') {
      steps {
        node(label: 'docker-stage') {
          powershell(script: 'asd', returnStatus: true, returnStdout: true)
          powershell(script: 'fasfa', returnStatus: true, returnStdout: true)
        }
        
      }
    }
    stage('Staging') {
      steps {
        powershell(script: 'asdad', returnStdout: true, returnStatus: true)
      }
    }
    stage('Live') {
      steps {
        powershell(script: 'faas', returnStatus: true, returnStdout: true)
      }
    }
  }
}